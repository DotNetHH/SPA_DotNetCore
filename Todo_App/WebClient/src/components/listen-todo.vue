<template>

    <div class="todo" @click="open()">

        <div class="art">
            {{ (todo.art || 'Eingang').toUpperCase() }} !!!!!!!
        </div>

        <div class="titel">
            {{ todo.titel }}
        </div>

    </div>

</template>

<script lang="ts">

    import { Vue, Component, Prop } from './imports';
    import { Todo } from '../models';

    @Component({ name: 'listen-todo' })
    export default class ListenTodo extends Vue {

        @Prop() todo?: Todo;

        open() {
            this.$router.push({ name: 'edit', params: { id: this.todo!.id!.toString() } })
        }
    }

</script>

<style lang="postcss" scoped>
    .todo {
        margin: 15px;
    }

    .todo:hover {
        cursor: pointer;
    }

    .art {
        padding: 5px;
        font-size: 10px;
        background-color: var(--c-primary);
        color: var(--c-bright);
    }
    .titel {
        padding: 10px 5px;
        background-color: var(--c-bright);
    }
</style>
