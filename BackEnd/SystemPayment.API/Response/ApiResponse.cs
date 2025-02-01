namespace SystemPayment.API.Response
{
	public class ApiResponse<T>
	{
		public T Data { get; set; }
		public bool Success { get; set; } 
		public List<string> Errors { get; set; } = new List<string>(); 
		public int StatusCode { get; set; }
		public ApiResponse(int statusCode = 200)
		{
			Success = true;
			StatusCode = statusCode;
		}
		public ApiResponse(T data, int statusCode = 200)
		{
			Data = data;
			Success = true;
			StatusCode = statusCode;
		}

		public ApiResponse(string[] errors, int statusCode = 400)
		{
			Errors = errors.ToList();
			Success = false;
			StatusCode = statusCode;
		}

		public ApiResponse(string error, int statusCode = 400) : this(new string[] { error }, statusCode)
		{
		}

	}
}
