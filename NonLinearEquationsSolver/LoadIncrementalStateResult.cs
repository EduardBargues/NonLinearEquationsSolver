namespace NonLinearEquationsSolver {
    internal class LoadIncrementalStateResult {

        internal LoadIncrementalState IncrementalState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal bool Success { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal string Message { get; set; }

        public LoadIncrementalStateResult( LoadIncrementalState incrementalState, bool success, string message ) {
            IncrementalState = incrementalState;
            Success = success;
            Message = message;
        }

        public LoadIncrementalStateResult() {

        }
    }
}
