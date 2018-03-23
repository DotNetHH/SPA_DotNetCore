import App from './components/app.vue';
import Start from './components/start.vue';
import EditTodo from './components/edit-todo.vue';

export const routes = [
    { name: 'start', path: '/', component: Start },
    { name: 'edit', path: '/edit/:id?', component: EditTodo },
];
