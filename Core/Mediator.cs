namespace Todo.Core
{
    public class Mediator
    {
        private static readonly Mediator _instance = new Mediator();
        private readonly Dictionary<string, List<Action<object>>> _subscribers
            = new Dictionary<string, List<Action<object>>>();

        public static Mediator Instance => _instance;

        public void Register(string message, Action<object> callback)
        {
            if (!_subscribers.ContainsKey(message))
            {
                _subscribers[message] = new List<Action<object>>();
            }
            _subscribers[message].Add(callback);
        }

        public void Notify(string message, object args)
        {
            if (_subscribers.ContainsKey(message))
            {
                foreach (var callback in _subscribers[message])
                {
                    callback(args);
                }
            }
        }
    }
}
