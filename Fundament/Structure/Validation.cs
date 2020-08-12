using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Integrant.Fundament.Structure
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

        public static List<Validation> One(ValidationResultType resultType, string message) =>
            new List<Validation> {new Validation(resultType, message)};
    }

    public class ValidationState<TStructure>
    {
        private readonly StructureInstance<TStructure> _structureInstance;

        private readonly object                                _cacheLock = new object();
        public           List<Validation>?                     StructureValidationCache;
        public           Dictionary<string, List<Validation>>? MemberValidationCache;

        private CancellationTokenSource? _tokenSource;

        public ValidationState(StructureInstance<TStructure> structureInstance)
        {
            _structureInstance = structureInstance;
        }

        public bool IsValidating { get; private set; }

        public bool Valid()
        {
            if (IsValidating)
                return false;

            lock (_cacheLock)
            {
                if (StructureValidationCache == null || MemberValidationCache == null)
                    return false;

                foreach (Validation validation in StructureValidationCache)
                {
                    if (validation.ResultType == ValidationResultType.Undefined ||
                        validation.ResultType == ValidationResultType.Invalid)
                        return false;
                }

                foreach ((_, List<Validation> validations) in MemberValidationCache)
                {
                    foreach (Validation validation in validations)
                    {
                        if (validation.ResultType == ValidationResultType.Undefined ||
                            validation.ResultType == ValidationResultType.Invalid)
                            return false;
                    }
                }

                return true;
            }
        }

        private static (List<Validation>, Dictionary<string, List<Validation>>) Validate
        (
            StructureInstance<TStructure> structure,
            TStructure                    value,
            CancellationToken             token
        )
        {
            token.ThrowIfCancellationRequested();

            var structureValidations = new List<Validation>();
            var memberValidations    = new Dictionary<string, List<Validation>>();

            if (structure.Structure.Validator != null)
                structureValidations = structure.Structure.Validator.Invoke(structure.Structure, value);

            token.ThrowIfCancellationRequested();

            foreach (IMemberInstance<TStructure> member in structure.AllMemberInstances())
            {
                token.ThrowIfCancellationRequested();

                List<Validation>? validations = member.Validations(value);
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
                var (structure, members) = Validate(_structureInstance, value, token);

                lock (_cacheLock)
                {
                    StructureValidationCache = structure;
                    MemberValidationCache    = members;
                }

                IsValidating = false;
                OnFinishValidating?.Invoke();
            }, token);
        }

        public event Action? OnInvalidation;
        public event Action? OnBeginValidating;
        public event Action? OnFinishValidating;

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

        public List<Validation>? GetStructureValidations()
        {
            lock (_cacheLock)
            {
                return StructureValidationCache;
            }
        }

        public List<Validation>? GetMemberValidations(string id)
        {
            lock (_cacheLock)
            {
                if (MemberValidationCache == null) return null;

                MemberValidationCache.TryGetValue(id, out List<Validation>? result);
                return result;
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