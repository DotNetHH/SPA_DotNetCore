<template>

    <div class="todos">
        <listen-todo v-for="(f, ii) of todos" :key="ii" :todo="f"></listen-todo>
    </div>

</template>

<script lang="ts">

    import { Vue, Component, State, Action } from './imports';
    import { Todo, LoadTodos } from '../models';
    import ListenTodo from './listen-todo.vue';

    @Component({ components: { ListenTodo } })
    export default class Start extends Vue {

        @State todos?: Todo[];

        async created() {
            await this.$store.dispatch(LoadTodos);
        }
    }

</script>

<style lang="postcss" scoped>
    @import "../../styles/_variables.css";

    .neues-todo {
        display: flex;
        flex-direction: column;
    }

    .todos {
        background-color: var(--c-bg-color);
        padding: 20px;
    }
</style>
