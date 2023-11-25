const uri = 'api/captureoneaction/';
enum CameraAction {
	invokeLiveView = 'invokeLiveView',
	invokeCapture = 'invokeCapture',
}

export function fetchInvokeLiveViewAction() {
	return fetch(uri, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json',
		},
		body: JSON.stringify({ action: CameraAction.invokeLiveView }),
	});
}

export function fetchCaptureAction() {
	return fetch(uri, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json',
		},
		body: JSON.stringify({ action: CameraAction.invokeCapture }),
	});
}
