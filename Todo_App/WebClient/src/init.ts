import Vue from 'vue';
import VueRouter from 'vue-router';

import { store } from './store';
import { routes } from './routes';

import App from './components/app.vue';

Vue.use(VueRouter);

new Vue({
    el: '#app',
    router: new VueRouter({ mode: 'history', routes }),
    store,
    render: h => h(App)
});
