<template>

    <div class="app">

        <div class="menu">
            <div class="links">
                <span>v{{appVersion}}</span>
                <router-link to="/">
                    <i class="fas fa-home"></i> Start</router-link>
            </div>
            <button class="add-todo" @click="add()">
                <i class="fas fa-plus"></i>
            </button>
        </div>

        <div class="main">
            <router-view></router-view>
        </div>

    </div>

</template>

<script lang="ts">

    import { Vue, Component } from 'vue-property-decorator';
    declare const APP_VERSION: string;

    @Component({ name: 'app' })
    export default class App extends Vue {

        appVersion = APP_VERSION;

        add() {
            this.$router.push('edit');
        }
    }

</script>

<style>
    @import "normalize.css";
    *,
    * pre {
        font-family: "Open Sans", sans-serif;
    }
</style>

<style lang="postcss" scoped>
    @import "../../styles/_variables.css";

    .menu {
        display: flex;
        justify-content: space-between;
    }
</style>
