namespace Integrant.Fundament
{
    public class Restrictor
    {
        public bool _allowed;

        public void AllowOnce()
        {
            _allowed = true;
        }
        
        public bool ShouldRender()
        {
            if (!_allowed) return false;
            
            _allowed = false;
            return _allowed;
        }
    }
}