<template>
  <div class="container">
    <header class="hero">
      <img :src="logo" />
      <hello></hello>
    </header>
  </div>
</template>

<script>
import Hello from '@/views/components/hello';
import logo from '@/assets/logo.png';

export default {
  name: 'App',
  components: {
    Hello
  },
  data() {
    return {
      logo
    };
  }
};
</script>
