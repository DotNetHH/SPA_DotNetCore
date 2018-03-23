import { mount } from '@vue/test-utils';
import ListeTodo from '../../src/components/listen-todo.vue';

describe('liste-todo', () => {

    it('is a Vue instance', () => {
        const wrapper = mount(ListeTodo, {
            propsData: {
                todo: { art: 'Woop' }
            }
        });
        expect(wrapper.isVueInstance()).toBeTruthy();
    });
});
