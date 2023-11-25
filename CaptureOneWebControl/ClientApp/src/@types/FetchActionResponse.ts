enum ResultStatus {
	Fail = 'Fail',
	Success = 'Success',
	Timeout = 'Timeout',
}
export default interface FetchActionResponse {
	status: ResultStatus;
	reason: string;
}
