import React, { useState } from 'react';
import Helmet from 'react-helmet';
import { useTransition, animated } from '@react-spring/web';

import './custom.css';
import './reset.css';
import CaptureButton from './components/CaptureButton';
import LiveViewButton from './components/LiveViewButton';
import ToastNotification from './components/ToastNotification';
import { v4 as uuid } from 'uuid';
import FetchActionResponse from './@types/FetchActionResponse';

interface Toast {
	id: string;
	fetchResponse: FetchActionResponse;
}

const transitionProps = {
	from: {
		y: '-100%',
		x: '50%',
		width: '5%',
		height: '5%',
		textOpacity: 0,
	},
	enter: () => async (next, cancel) => {
		await next({
			y: '30%',
			x: '50%',
			width: '5%',
			height: '5%',
			textOpacity: 0,
			onStart: (_r, spring) => {
				setTimeout(() => {
					spring.update({
						y: '30%',
						x: '50%',
						width: '70%',
						height: '5%',
						textOpacity: 1,
					});
					spring.start();
				}, 100);
			},
		});
	},
	leave: () => async (next) => {
		await next({
			y: '-120%',
			x: '50%',
			width: '5%',
			height: '5%',
			textOpacity: 0,
		});
	},
	config: {
		mass: 0.7,
		tension: 200,
		friction: 10,
	},
};

export default function App() {
	const [toastTexts, setToastTexts] = useState<Toast[]>([]);
	const AnimatedToast = animated(ToastNotification);
	const transitions = useTransition(toastTexts, transitionProps);

	function removeToastAfterSeconds(toast: Toast, secondsInMs: number) {
		setTimeout(() => {
			setToastTexts((prev) => {
				return prev.filter((t) => t.id !== toast.id);
			});
		}, secondsInMs);
	}

	function setToastMessage(response: FetchActionResponse) {
		const newToast: Toast = { id: uuid(), fetchResponse: response };
		setToastTexts((prev) => [...prev, newToast]);
		removeToastAfterSeconds(newToast, 1000);
	}

	return (
		<>
			<Helmet>
				<meta
					name='viewport'
					content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no'></meta>
			</Helmet>
			<div className='buttonsGroup'>
				<CaptureButton sendResponse={setToastMessage} />
				<LiveViewButton sendResponse={setToastMessage} />
			</div>
			{transitions((style, item) => (
				<AnimatedToast
					style={style}
					response={item}
				/>
			))}
		</>
	);
}
