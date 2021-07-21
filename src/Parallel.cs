using System.Threading.Tasks;
using LanguageExt;

namespace SpikeLanguageExt
{
    public class Result
    {
        public int Error { get; set; }
        public string Content { get; set; }
        
        public static Result some(string content)
        {
            return new Result {
                Content = content
            };
        }

        public static Result none(int error)
        {
            return new Result {
                Error = error
            };
        }
    }
    public class ParallelServices
    {
        private ServiceOne serviceOne = new ServiceOne();
        private ServiceTwo serviceTwo = new ServiceTwo();

        public async Task<Result> DoWork(string id)
        {
            var result = (from x in serviceOne.DoWork(id)
                          from y in serviceTwo.DoWork(id)
                          select x + y);

            return (await result)
                        .Match(value => Result.some(value),
                               err => Result.none(err));
        }

        public async Task<Result> DoWorkAsync(string id)
        {
            var first = serviceOne.DoWork(id);
            var second = serviceTwo.DoWork(id);
            await Task.WhenAll(first, second);

            var result = (from x in first
                          from y in second
                          select x + y);

            return (await result)
                        .Match(value => Result.some(value),
                               err => Result.none(err));
        }
    }

    public class ServiceOne
    {
        public async Task<Either<int, string>> DoWork(string id)
        {
            Either<int, string> result;
            if (id == "1")
                result = 1;            
            else
                result = $"ServiceOne-{id}";
            await Task.Delay(1000);
            return await Task.FromResult(result);
        }
    }

    public class ServiceTwo
    {
        public async Task<Either<int, string>> DoWork(string id)
        {
            Either<int, string> result;
            if (id == "2")
                result = 2;            
            else
                result = $"ServiceTwo-{id}";
            return await Task.FromResult(result);
        }
    }
}