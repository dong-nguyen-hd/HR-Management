<template>
  <div class="hello">
    ...
    <!-- Add a test button -->
    <ui-button icon="add" @click="$alert('Hello BalmUI')">Click Me</ui-button>
    <p>Testing</p>
  </div>
</template>

<script>
export default {
  name: 'Hello',
  data() {
    return {
      msg: 'Hello Vue App'
    };
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h1 {
  font-weight: normal;
}

a {
  color: #42b983;
}
</style>
