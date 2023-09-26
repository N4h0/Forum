using Forum.Models;

namespace Forum.ViewModels
{
    public class ThreadListViewModel
    {
        //Thread kan referere til både System.Thread og Models.Thread, derfor må vi klarere ...:
        public IEnumerable<Models.Thread> Threads { get; set; }
        public string? CurrentViewName;

        public ThreadListViewModel(IEnumerable<Models.Thread> threads, string? currentViewName)
        {
            Threads = threads;
            CurrentViewName = currentViewName;
        }
    }
}
