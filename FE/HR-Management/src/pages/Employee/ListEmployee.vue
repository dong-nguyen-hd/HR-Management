<template>
  <q-page class="full-height full-width flex flex-center">
    <div class="container full-height full-width">
      <div class="table-component full-width flex flex-center">
        <div class="new-item q-mb-md flex justify-end" style="width: 96%">
          <q-btn color="primary" label="New employee" />
        </div>

        <q-table
          card-class="bg-accent text-black"
          table-class="text-white bg-primary"
          table-header-class="text-black"
          class="table-content"
          :rows="listEmployee"
          :columns="headerTable"
          row-key="id"
          flat
          bordered
          dark
          virtual-scroll
        >
          <template v-slot:body-cell-avatar="props">
            <q-td :props="props">
              <q-avatar>
                <img :src="props.value" />
              </q-avatar>
            </q-td>
          </template>

          <template v-slot:body-cell-skill="props">
            <q-td :props="props">
              <div v-if="props.value.length">
                <div v-for="(item, index) in props.value" :key="index">
                   <q-badge :color="index % 2 == 0 ? 'blue-10' : 'teal-8' ">
                    <span>{{ props?.value[index].categoryName }}:</span>
                  </q-badge>
                 <span>
                    {{ props?.value[index]?.technology[0]?.name?` ${props?.value[index]?.technology[0]?.name},`:'' }}{{ props?.value[index]?.technology[1]?.name?` ${props?.value[index]?.technology[1]?.name},`:'' }}{{ props?.value[index]?.technology[2]?.name?` ${props?.value[index]?.technology[2]?.name},`:'' }}...
                  </span>
                </div>
              </div>
            </q-td>
          </template>
        </q-table>
      </div>
    </div>
  </q-page>
</template>

<script>
import { defineComponent } from "vue";
import { mapGetters, mapActions, mapMutations } from "vuex";
import { useQuasar } from "quasar";
import { api } from "src/boot/axios";

export default defineComponent({
  name: "List Employee",

  data() {
    return {
      pagination: {
        page: 1,
        pageSize: 10,
        firstPage: 1,
        lastPage: 0,
        nextPage: 0,
        previousPage: null,
        totalPages: 0,
        totalRecords: 0,
      },

      listEmployee: [],
      headerTable: [
        {
          name: "index",
          label: "#",
          field: "index",
        },
        {
          name: "staffId",
          align: "left",
          label: "Staff ID",
          field: "staffId",
        },
        {
          name: "office",
          align: "left",
          label: "Office",
          field: (row) => row.location.name,
        },
        {
          name: "avatar",
          align: "center",
          label: "",
          field: (row) => row.avatar.thumbnail,
        },
        {
          name: "fullName",
          align: "left",
          label: "Full Name",
          field: (row) => `${row.firstName} ${row.lastName}`,
        },
        {
          name: "skill",
          align: "left",
          label: "Skills",
          field: (row) => row.categoryPerson,
        },
        {
          name: "fullName",
          align: "left",
          label: "Actions",
          field: "Test",
        },
      ],
    };
  },
  methods: {
    ...mapActions("auth", ["useRefreshToken", "validateToken"]),
    ...mapMutations("auth", ["setInformation"]),

    async getEmployee() {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(
          `/api/v1/person?page=${this.pagination.page}&pageSize=${this.pagination.pageSize}`
        )
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
        this.listEmployee = result.resource;

        this.listEmployee.forEach((row, index) => {
          row.index = index + 1;
        });
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
  },
  computed: {
    ...mapGetters("auth", ["getInformation"]),
  },
  async created() {
    await this.getEmployee();
  },
  mounted() {
    const $q = useQuasar();
  },
});
</script>

<style lang="scss" scoped>
.table-component {
  // width: 100%;
  // height: 100%;
}

.table-content {
  /* height or max-height is important */
  max-height: 600px;
  width: 96%;
}
</style>

<style lang="scss">
.table-content {
  .q-table__top,
  .q-table__bottom,
  thead tr:first-child th {
    /* bg color is important for th; just specify one */
    background-color: $accent !important;
  }

  thead tr th {
    position: sticky;
    z-index: 1;
  }

  thead tr:first-child th {
    top: 0;
  }

  /* this is when the loading indicator appears */
  &.q-table--loading thead tr:last-child th {
    /* height of all previous header rows */
    top: 48px;
  }
}

/* Scroll bar */
::-webkit-scrollbar {
  width: 10px;
  height: 10px;
}

/* Track */

::-webkit-scrollbar-track {
  box-shadow: inset 0 0 5px $secondary;
  border-radius: 10px;
}

/* Handle */

::-webkit-scrollbar-thumb {
  background: $accent;
  border-radius: 10px;
}

/* Handle on hover */

// ::-webkit-scrollbar-thumb:hover {
//     background: $secondary;
// }
</style>
