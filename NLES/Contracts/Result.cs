using System.Collections.Generic;
using System.Linq;

namespace NLES.Contracts
{
    public class Result<T>
    {
        public bool IsSuccess => !Errors.Any();
        public T Value { get; internal set; } = default;
        public List<Error> Errors { get; internal set; } = new List<Error>();
    }
}
