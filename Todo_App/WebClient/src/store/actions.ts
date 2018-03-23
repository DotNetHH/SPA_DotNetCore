import axios from 'axios';
import { ActionContext } from 'vuex';
import { AppState } from '../models';

import { LoadTodos, SaveTodo, GetTodo, GetTodoArten } from '../models/storeActions';
import { Todo } from '../models/todo';
import { ADD_TODO, SET_TODOS, SET_CURRENT_TODO, SET_TODO_ARTEN, UPDATE_TODO } from './mutations';

axios.defaults.baseURL = window.location.origin + '/api/requests/';

export const actions = {

    async [LoadTodos]({ commit, state }: ActionContext<AppState, AppState>) {

        // TODO test! // Logik in den Aufruf verschieben?
        if (state.todos && state.todos.length > 0) {
            return;
        }

        const result = await axios.post('alletodos', {});
        commit(SET_TODOS, result.data);
    },

    async [GetTodo]({ commit }: ActionContext<AppState, AppState>, todoId: number) {

        const requestBody = { id: todoId };
        const result = await axios.post('gettodo', requestBody);
        commit(SET_CURRENT_TODO, result.data);
    },

    async [SaveTodo]({ commit }: ActionContext<AppState, AppState>, todo: Todo) {

        if (!todo.id) {

            const requestBody = { titel: todo.titel, text: todo.text };
            const result = await axios.post('todoanlegen', requestBody);
            commit(ADD_TODO, result.data);
        }
        else {
            // update
            const requestBody = { id: todo.id, titel: todo.titel, text: todo.text, artName: todo.art };
            const result = await axios.post('todoaendern', requestBody);
            commit(UPDATE_TODO, todo);
        }
    },

    async [GetTodoArten]({ commit }: ActionContext<AppState, AppState>) {

        const result = await axios.post('gettodoarten', {});
        commit(SET_TODO_ARTEN, result.data);
    }
};
