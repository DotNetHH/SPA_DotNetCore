import { mount, Wrapper, createLocalVue } from '@vue/test-utils';
import Vuex, { Store } from 'vuex';
import EditFrament from '../../src/components/edit-todo.vue';
import { Test } from './helpers';
import { AppState } from '../../src/models';

const localVue = createLocalVue();

localVue.use(Vuex);

describe('edit-todo', () => {
    let wrapper: Wrapper<EditFrament>;
    let test: Test;
    let store: Store<AppState>;

    beforeEach(() => {
        store = new Vuex.Store<AppState>({
            state: {
                currentTodo: null,
                todos: [],
                todoArten: []
            }
        });
        wrapper = mount(EditFrament, { store, localVue });
        test = new Test(wrapper);
    });

    it('sollte bei neuem Todo "Anlegen" anzeigen', () => {

        test.see('Anlegen', 'button');
    });

    it('sollte nicht speicherbar sein ohne Titel', () => {

        test.see('', '[data-test="titel"]');

        const button = wrapper.find('button');
        expect(button.attributes()['disabled']).toEqual('disabled');
    });

    it('sollte mit Titel speicherbar sein', () => {

        test.type('[data-test="titel"]', 'Woop');

        const button = wrapper.find('button');
        expect(button.attributes()['disabled']).toBeUndefined();
    });
});

