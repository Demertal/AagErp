using Prism.Events;

namespace ProductModul
{
    public class BoolSentEvent : PubSubEvent<bool>
    {
    }

    public class IsReadySentEvent : PubSubEvent<bool>
    {
    }
}
