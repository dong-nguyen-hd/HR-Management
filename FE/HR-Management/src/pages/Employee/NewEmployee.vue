<template>
  <q-page class="full-height full-width flex flex-center">
    <div class="container full-height full-width">
        New Test
    </div>
  </q-page>
</template>

<script>
import { defineComponent } from "vue";
import { mapGetters, mapActions, mapMutations } from "vuex";
import { useQuasar } from "quasar";
import { api } from "src/boot/axios";

export default defineComponent({
  name: "New Employee",

  data() {
    return {
      listCategory: [],
      listLocation: [],
    };
  },
  methods: {
    ...mapActions("auth", ["useRefreshToken", "validateToken"]),

    async getCategory() {
      try {
        // Request API
        let result = await api
          .get(`/api/v1/category`)
          .then((response) => {
            return response.data;
          })
          .catch(function (error) {
            // Checking if throw error
            if (error.response) {
              // Server response
              return error.response.data;
            } else {
              // Server not working
              let temp = { success: false, message: ["Server Error!"] };
              return temp;
            }
          });

        if (result.success) {
          this.listCategory = result.resource;
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.loadingData = false;
      }
    },
    async getLocation() {
      try {
        // Request API
        let result = await api
          .get(`/api/v1/location`)
          .then((response) => {
            return response.data;
          })
          .catch(function (error) {
            // Checking if throw error
            if (error.response) {
              // Server response
              return error.response.data;
            } else {
              // Server not working
              let temp = { success: false, message: ["Server Error!"] };
              return temp;
            }
          });

        if (result.success) {
          this.listLocation = result.resource;
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.loadingData = false;
      }
    },
  },
  computed: {
    ...mapGetters("auth", ["getInformation"]),
  },
  async created() {
    let isValid = await this.validateToken();
    if (!isValid) this.$router.replace("/login");

    await Promise.all([this.getCategory(), this.getLocation()]);
  },
  mounted() {
    const $q = useQuasar();
  },
});
</script>

<style lang="scss" scoped></style>
