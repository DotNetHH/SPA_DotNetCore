import Vue from 'vue';
import Vuex, { Store, ActionTree } from 'vuex';
import { Todo, AppState } from '../models';
import { actions } from './actions';
import { mutations } from './mutations';

Vue.use(Vuex);

const state: AppState = {
    currentTodo: null,
    todos: [],
    todoArten: []
};

const store = new Vuex.Store<AppState>({
    state,
    mutations,
    actions
});

export { store };
