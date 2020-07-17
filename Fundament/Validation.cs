using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Fundament
{
    public class Validation
    {
        public readonly ValidationResultType ResultType;
        public readonly string               Message;

        public Validation(ValidationResultType resultType, string message)
        {
            ResultType = resultType;
            Message    = message;
        }

        internal RenderFragment Render() => builder =>
        {
            int          seq     = -1;
            List<string> classes = new List<string> {"Fundament.Validation"};

            switch (ResultType)
            {
                case ValidationResultType.Invalid:
                    classes.Add("Invalid");
                    break;
                case ValidationResultType.Warning:
                    classes.Add("Warning");
                    break;
                case ValidationResultType.Valid:
                    classes.Add("Valid");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            builder.OpenElement(++seq, "div");
            builder.AddAttribute(++seq, "class", string.Join(' ', classes));

            builder.OpenElement(++seq, "span");
            builder.AddAttribute(++seq, "class", "ValidationText");
            builder.AddContent(++seq, Message);
            builder.CloseElement();

            builder.CloseElement();
        };

        public static List<Validation> One(ValidationResultType resultType, string message) =>
            new List<Validation> {new Validation(resultType, message)};
    }

    public class ValidationState<TStructure>
    {
        public List<Validation>?                     StructureValidationCache;
        public Dictionary<string, List<Validation>>? MemberValidationCache;

        public bool IsValidating { get; private set; }
        
        private readonly Structure<TStructure> _structure;

        public ValidationState(Structure<TStructure> structure)
        {
            _structure = structure;
        }

        private readonly object _cacheLock = new object();

        private CancellationTokenSource? _tokenSource;

        private static (List<Validation>, Dictionary<string, List<Validation>>) Validate
        (
            Structure<TStructure> structure,
            TStructure            value,
            CancellationToken     token
        )
        {
            token.ThrowIfCancellationRequested();

            var structureValidations = new List<Validation>();
            var memberValidations    = new Dictionary<string, List<Validation>>();

            if (structure.StructureValidator != null)
                structureValidations = structure.StructureValidator.Invoke(structure, value);

            token.ThrowIfCancellationRequested();

            memberValidations ??= new Dictionary<string, List<Validation>>();

            foreach (IMember<TStructure> member in structure.AllMembers())
            {
                token.ThrowIfCancellationRequested();

                List<Validation>? validations = member.Validations(structure, value);
                if (validations == null) continue;

                token.ThrowIfCancellationRequested();

                if (!memberValidations.ContainsKey(member.ID))
                    memberValidations[member.ID] = new List<Validation>();

                memberValidations[member.ID].AddRange(validations);
            }

            return (structureValidations, memberValidations);
        }

        private void BeginValidations(TStructure value)
        {
            IsValidating = true;
            OnBeginValidating?.Invoke();
            _tokenSource?.Cancel();
            _tokenSource = new CancellationTokenSource();
            CancellationToken token = _tokenSource.Token;

            Task.Run(() =>
            {
                Console.WriteLine("Validating task: STARTED");

                var (structure, members) = Validate(_structure, value, token);

                Console.WriteLine("Validating task: FINISHED");

                lock (_cacheLock)
                {
                    StructureValidationCache = structure;
                    MemberValidationCache    = members;
                }

                IsValidating = false;
                OnFinishValidatingStructure?.Invoke();
            }, token);
        }

        public event Action? OnInvalidation;
        public event Action? OnBeginValidating;
        public event Action? OnFinishValidatingStructure;

        internal void Invalidate()
        {
            lock (_cacheLock)
            {
                StructureValidationCache = null;
                MemberValidationCache    = null;
                OnInvalidation?.Invoke();
            }
        }
        
        internal void ValidateStructure(TStructure value)
        {
            BeginValidations(value);
        }

        internal List<Validation>? GetMemberValidations(string id)
        {
            lock (_cacheLock)
            {
                if (MemberValidationCache?.ContainsKey(id) == true)
                    return MemberValidationCache[id];
                return null;
            }
        }
    }

    public enum ValidationResultType
    {
        Undefined,
        Invalid,
        Warning,
        Valid,
    }
}