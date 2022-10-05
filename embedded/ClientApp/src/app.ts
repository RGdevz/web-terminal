import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import PrimeVue from 'primevue/config';
import Dialog from 'primevue/dialog';
import Toast from 'primevue/toast'
import ToastService from 'primevue/toastservice';
import 'primevue/resources/primevue.min.css'
import 'primevue/resources/themes/saga-blue/theme.css'
import 'primeicons/primeicons.css'
import 'primeflex/primeflex.css';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';

import TheApp from './pages/app.vue'
import {singleton} from "./singleton";
import Ripple from 'primevue/ripple';


  export function init(){

  	const router = createRouter({
	 	history: createWebHistory(),


		 routes: [



			{ path: '/',  component: () => import('./pages/hello.vue') },
			{ path: '/auth/login',  component: () => import('./pages/login.vue') },


		],
 	}
 	)




	const app = createApp(TheApp).use(router).use(PrimeVue, {ripple: true}).use(ToastService)



	app.component('Dialog', Dialog);
	app.component('Toast', Toast);
	app.component('Button', Button);
			app.component('InputText', InputText);
			app.directive('ripple', Ripple);

	app.mount('#app')


}

