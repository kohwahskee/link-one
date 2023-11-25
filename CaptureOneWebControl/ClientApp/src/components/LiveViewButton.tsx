import React from 'react';
// @ts-ignore
import LiveViewIconLeft from '../assets/LiveViewIconLeft.svg';
// @ts-ignore
import LiveViewIconRight from '../assets/LiveViewIconRight.svg';
// @ts-ignore
import LiveViewIconText from '../assets/LiveViewIconText.svg';
import { fetchInvokeLiveViewAction } from './fetchActions';
import FetchActionResponse from '../@types/FetchActionResponse';

interface Props {
	sendResponse: (response: FetchActionResponse) => void;
}

export default function LiveViewButton({ sendResponse }: Props) {
	const defaultSuccessMessage = 'Invoked Live View';
	return (
		<button
			id='liveViewButton'
			className='button'
			onContextMenu={(e) => e.preventDefault()}
			onClick={async () => {
				const result = (await (
					await fetchInvokeLiveViewAction()
				).json()) as FetchActionResponse;
				sendResponse({
					status: result.status,
					reason:
						result.status === 'Fail' ? result.reason : defaultSuccessMessage,
				});
			}}>
			<div id='liveViewIcon'>
				<img
					id='liveViewIconLeft'
					alt='Live View Button'
					src={LiveViewIconLeft}
				/>
				<img
					id='liveViewIconText'
					alt='Live View Button'
					src={LiveViewIconText}
				/>
				<img
					id='liveViewIconRight'
					alt='Live View Button'
					src={LiveViewIconRight}
				/>
			</div>
		</button>
	);
}
