import React from 'react';
// @ts-ignore
import CaptureIconInner from '../assets/CaptureIconInner.svg';
// @ts-ignore
import CaptureIconOuter from '../assets/CaptureIconOuter.svg';
import { fetchCaptureAction } from './fetchActions';
import FetchActionResponse from '../@types/FetchActionResponse';

interface Props {
	sendResponse: (response: FetchActionResponse) => void;
}

export default function CaptureButton({ sendResponse }: Props) {
	const defaultSuccessMessage = 'Invoked Capture Button';
	return (
		<button
			id='captureButton'
			className='button'
			onContextMenu={(e) => e.preventDefault()}
			onClick={async () => {
				const result = (await (
					await fetchCaptureAction()
				).json()) as FetchActionResponse;
				sendResponse({
					status: result.status,
					reason:
						result.status === 'Fail' ? result.reason : defaultSuccessMessage,
				});
			}}>
			<div id='captureIcon'>
				<img
					id='captureIconInner'
					alt='Capture Button'
					src={CaptureIconInner}
				/>
				<img
					id='captureIconOuter'
					alt='Capture Button'
					src={CaptureIconOuter}
				/>
			</div>
		</button>
	);
}
