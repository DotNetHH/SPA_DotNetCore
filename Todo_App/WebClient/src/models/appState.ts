import { Todo, TodoArt } from './index';

export interface AppState {
    todos: Todo[];
    currentTodo: Todo | null;
    todoArten: TodoArt[];
}
