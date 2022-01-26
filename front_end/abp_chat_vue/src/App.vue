<template>
  <div id="app">
    <router-view v-if="!$route.meta.keepAlive"></router-view>
    <router-view v-if="$route.meta.keepAlive"></router-view>
  </div>
</template>

<script>
export default {
  name: "App",
  components: {
  },
  data() {
    return {};
  },
  created() {}
};
</script>

<style>
#app {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  background: url("./assets/static/back.jpg");
  background-size: 100% 100%;
  background-position: center center;
}
</style>
