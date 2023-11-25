import { animated } from '@react-spring/web';
import React from 'react';

export default function ToastNotification({ style, response }) {
	const fetchResponse = response.fetchResponse;

	return (
		<animated.div
			className='toastNotification'
			style={{
				...style,
				backgroundColor:
					fetchResponse.status === 'Success'
						? 'var(--color-capture-one-blue)'
						: 'var(--color-red)',
			}}>
			<span
				style={{
					opacity: style.textOpacity,
				}}>
				{fetchResponse.reason}
			</span>
		</animated.div>
	);
}
