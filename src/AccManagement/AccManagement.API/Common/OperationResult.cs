using System.Collections.Generic;

namespace AccManagement.API.Common {
    public class OperationResult {
        private readonly List<string> _errors;
        public IReadOnlyCollection<string> Errors => _errors;
        
        public bool Success => _errors.Count == 0;
        public bool HasErrors => _errors.Count > 0;

        protected OperationResult() {
            _errors = new List<string>();
        }

        private OperationResult(IEnumerable<string> errors) {
            _errors = new List<string>(errors);
        }

        private OperationResult(string error) {
            _errors = new List<string> {error};
        }

        public static OperationResult Ok() => new OperationResult();
        public static OperationResult Ok<T>(T data) => new OperationResult<T>(data);
        public static OperationResult Fail(string error) => new OperationResult(error);
        public static OperationResult Fail(IEnumerable<string> errors) => new OperationResult(errors);

        public void AddError(string error) {
            _errors.Add(error);
        }
    }

    public class OperationResult<T> : OperationResult {
        public T Data { get; }

        public OperationResult(T data) {
            Data = data;
        }
    }
}