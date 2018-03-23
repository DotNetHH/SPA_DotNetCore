export class Test {

    constructor(private wrapper: any) {
    }

    see(text: string, selector: string) {

        let wrap = selector ? this.wrapper.find(selector) : this.wrapper;

        expect(wrap.html()).toContain(text);
    }

    type(selector: string, text: string) {

        const node = this.wrapper.find(selector);
        const input = node.element as HTMLInputElement;

        input.value = text;
        node.trigger('input');
    };

    click(selector: string) {

        this.wrapper.find(selector).trigger('click');
    }
}
