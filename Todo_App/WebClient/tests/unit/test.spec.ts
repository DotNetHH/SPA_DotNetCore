import { Vue } from 'vue-property-decorator';
import { mount, Wrapper } from '@vue/test-utils';
import App from '../../src/components/app.vue';
import VueRouter from 'vue-router';

describe('App', () => {
    let wrapper: Wrapper<App>;

    beforeEach(() => {
        wrapper = mount(App, {
            stubs: ['router-link', 'router-view']
        });
    });

    it('is a Vue instance', () => {
        expect(wrapper.isVueInstance()).toBeTruthy();
    });

    it('contains an .app-div', () => {
        const appDiv = wrapper.find('div.app');
        expect(appDiv).not.toBeNull();
    });
});
