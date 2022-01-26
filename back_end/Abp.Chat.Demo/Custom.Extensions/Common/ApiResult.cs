using System.Threading.Tasks;

namespace Custom.Extensions
{
    public class ApiResult<T>
    {
        public int Code { get; }
        public T Data { get; }
        public ApiResult(T data, int code = 200)
        {
            Code = code;
            Data = data;
        }
        public static ApiResult<T> GetResult(T _data, int code = 200)
        {
            return new ApiResult<T>(_data, code);
        }
        public static Task<ApiResult<T>> GetResultAsync(T _data, int code = 200)
        {
            return Task.FromResult(new ApiResult<T>(_data, code));
        }

    }
}
