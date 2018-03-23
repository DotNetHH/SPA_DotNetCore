<template>

    <div class="neues-todo">

        <select v-model="todo.art">
            <option v-for="option in todoArten" v-bind:key="option.id" v-bind:value="option.name">
                {{ option.name }}
            </option>
        </select>

        <input data-test="titel" type="text" v-model="todo.titel" placeholder="Titel" autofocus>
        <textarea data-test="text" v-model="todo.text" placeholder="Text"></textarea>

        <button data-test="anlegen" :disabled="!todo.titel" @click="anlegen">
            {{ isNew ? 'Anlegen': 'Speichern' }}
        </button>

    </div>

</template>

<script lang="ts">
    import { Vue, Component, State, Action } from './imports';
    import { Todo, TodoArt, GetTodo, SaveTodo, GetTodoArten } from '../models';

    @Component({ name: 'edit-todo', components: {} })
    export default class EditTodo extends Vue {

        isNew = true;
        todo: Todo = { id: null, art: 'Eingang', titel: '', text: '' };

        @State currentTodo?: Todo;
        @State todoArten?: TodoArt[];

        async created() {

            await this.$store.dispatch(GetTodoArten);

            if (!this.$route.params.id) return;
            this.isNew = false;

            var id = parseInt(this.$route.params.id)
            await this.$store.dispatch(GetTodo, id);

            // TODO thats stupid!
            this.todo = this.currentTodo!;
        }

        async anlegen() {
            await this.$store.dispatch(SaveTodo, this.todo);
            this.todo = { id: 0, art: 'Eingang', titel: '', text: '' };
            this.$router.push({ name: 'start' });
        }
    }

</script>

<style lang="postcss" scoped>
    @import "../../styles/_variables.css";

    .neues-todo {
        display: flex;
        flex-direction: column;
    }
</style>
