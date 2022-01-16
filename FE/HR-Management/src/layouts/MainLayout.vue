<template>
  <q-layout view="hHh lpR fFf">
    <q-header reveal class="bg-secondary text-white">
      <q-toolbar>
        <q-btn dense flat round icon="menu" @click="toggleLeftDrawer" />

        <q-toolbar-title>
          <q-avatar>
            <img src="https://cdn.quasar.dev/logo-v2/svg/logo-mono-white.svg" />
          </q-avatar>
          Title
        </q-toolbar-title>
        <q-btn stretch flat @click="logOut">Log out</q-btn>
      </q-toolbar>
    </q-header>

    <q-drawer
      show-if-above
      v-model="leftDrawerOpen"
      side="left"
      bordered
      behavior="desktop"
      class="bg-primary"
    >
      <!-- drawer content -->
      <div class="q-py-md bg-primary text-white">
        <q-list style="max-width: 318px">
          <q-item clickable v-ripple>
            <q-item-section avatar>
              <q-icon color="text" name="bluetooth" />
            </q-item-section>

            <q-item-section>Icon as avatar</q-item-section>
          </q-item>

          <q-item clickable v-ripple>
            <q-item-section avatar>
              <q-icon color="text" name="bluetooth" />
            </q-item-section>

            <q-item-section>Icon as avatar</q-item-section> </q-item
          ><q-item clickable v-ripple>
            <q-item-section avatar>
              <q-icon color="text" name="bluetooth" />
            </q-item-section>

            <q-item-section>Icon as avatar</q-item-section>
          </q-item>

          <q-separator class="bg-grey-7" />
        </q-list>
      </div>
    </q-drawer>

    <q-page-container class="bg-primary">
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
import { defineComponent } from 'vue'
import { mapGetters } from 'vuex'

export default defineComponent({
  name: 'MainLayout',

  data () {
    return {
      leftDrawerOpen: false,
    }
  },
  methods: {
    toggleLeftDrawer () {
      this.leftDrawerOpen = !this.leftDrawerOpen
    },
    logOut () {
      this.$store.dispatch('auth/logOut')
      this.$router.push('/')
    }
  },
  computed: {
    ...mapGetters('auth', ['isAuthenticated'])
  }
})
</script>

<style lang="scss" scoped>

</style>
