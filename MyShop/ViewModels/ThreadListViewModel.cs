using Forum.Models;

namespace Forum.ViewModels
{
    public class ThreadListViewModel
    {
        //Topic kan referere til både System.Topic og Models.Topic, derfor må vi klarere ...:
        public IEnumerable<Models.Topic> Topics { get; set; }
        public string? CurrentViewName;

        public ThreadListViewModel(IEnumerable<Models.Topic> topics, string? currentViewName)
        {
            Topics = topics;
            CurrentViewName = currentViewName;
        }
    }
}
