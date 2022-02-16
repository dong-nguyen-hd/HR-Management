<template>
  <q-page class="full-height full-width flex flex-center">
    <div class="container">
      <div class="avatar flex flex-center q-pt-md column">
        <q-avatar size="200px">
          <img :src="accountInfor.avatar.original" />
        </q-avatar>
        <q-file
          dense
          standout
          input-style="width: 150px; max-height: 40px"
          label="Upload image here!"
          v-model="avatarImage"
          class="q-mt-sm"
        >
          <template v-slot:prepend>
            <q-icon name="attach_file" />
          </template>
        </q-file>
      </div>
      <div class="userName">
        <q-input
          disable
          standout
          v-model="accountInfor.userName"
          type="text"
          prefix="User Name:"
        >
          <template v-slot:prepend>
            <q-icon name="fas fa-user" />
          </template>
        </q-input>
      </div>
      <div class="name">
        <q-input
          standout
          v-model="accountInfor.name"
          type="text"
          prefix="Name:"
        >
          <template v-slot:prepend>
            <q-icon name="fas fa-user" />
          </template>
        </q-input>
      </div>
      <div class="email">
        <q-input
          standout
          v-model="accountInfor.email"
          type="email"
          prefix="Email:"
        >
          <template v-slot:prepend>
            <q-icon name="mail" />
          </template>
        </q-input>
      </div>
      <div class="btn-save">
        <q-btn color="primary" label="Save" />
      </div>
    </div>
  </q-page>
</template>

<script>
import { defineComponent } from "vue";
import { mapGetters, mapActions } from "vuex";

export default defineComponent({
  name: "Update Personal Information",

  data() {
    return {
      accountInfor: null,

      avatarImage: null,
    };
  },
  methods: {
    ...mapActions("auth", ["logOut"]),

    async logOutButton() {
      try {
        await this.logOut();
      } finally {
        this.$router.replace("/login");
      }
    },
  },
  computed: {
    ...mapGetters("auth", ["getInformation"]),
  },
  created() {
    this.accountInfor = Object.assign({}, this.getInformation);
  },
});
</script>

<style lang="scss" scoped>
.container {
  background-color: $accent;
  height: 600px;
  width: 460px;
  border-radius: 10px;
}
</style>
