import { AppState } from '../models/appState';
import { Todo, TodoArt } from '../models';

export const SET_TODOS = 'SET_TODOS';
export const ADD_TODO = 'ADD_TODO';
export const UPDATE_TODO = 'UPDATE_TODO';
export const SET_CURRENT_TODO = 'SET_CURRENT_TODO';
export const SET_TODO_ARTEN = 'SET_TODO_ARTEN';

export const mutations = {

    [SET_TODOS](state: AppState, todos: Todo[]) {
        state.todos = todos;
    },

    [ADD_TODO](state: AppState, newTodo: Todo) {
        state.todos.unshift(newTodo);
    },

    [UPDATE_TODO](state: AppState, todo: Todo) {
        const replaceId = state.todos.findIndex(x => x.id === todo.id);
        state.todos[replaceId] = todo;
    },

    [SET_CURRENT_TODO](state: AppState, todo: Todo) {
        state.currentTodo = todo;
    },

    [SET_TODO_ARTEN](state: AppState, todoArten: TodoArt[]) {
        state.todoArten = todoArten;
    }
};
