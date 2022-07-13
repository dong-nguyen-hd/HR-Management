<template>
  <q-page class="full-height full-width flex flex-center">
    <div class="container full-height full-width">
      <q-dialog v-model="showDelete" :persistent="deleteProcess">
        <q-card>
          <q-card-section class="row items-center">
            <span class="text-h6">Delete {{ getNameDelete }}?</span>
          </q-card-section>

          <q-separator />

          <q-card-section>
            <span
              >This can’t be undone and it will be removed from database.</span
            >
          </q-card-section>

          <q-card-actions align="right">
            <q-btn
              flat
              :disable="deleteProcess"
              label="Cancel"
              color="primary"
              v-close-popup
            />
            <q-btn
              flat
              label="Delete"
              color="negative"
              @click="deletePosition"
              :loading="deleteProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <q-dialog v-model="showPosition" :persistent="getPersistentPosition">
        <q-card style="width: 400px">
          <q-card-section class="row items-center">
            <span v-show="!showEdit" class="text-h6">Add Position</span>
            <span v-show="showEdit" class="text-h6">Edit Position</span>
          </q-card-section>

          <q-separator />

          <q-card-section>
            <div class="q-mt-sm">
              <q-input
                ref="addCategoryOneRef"
                standout
                clearable
                maxlength="250"
                v-model="tempPositionResource.name"
                type="text"
                label="Name:"
                :label-color="colorFocusCategory[0]"
                @focus="colorFocusCategory[0] = 'white'"
                @blur="colorFocusCategory[0] = ''"
                :rules="[(val) => !!val || 'Name is required']"
                lazy-rules="ondemand"
                hide-bottom-space
              >
              </q-input>
            </div>
          </q-card-section>

          <q-card-actions align="right">
            <q-btn
              flat
              :disable="positionProcess"
              label="Cancel"
              color="primary"
              v-close-popup
            />
            <q-btn
              v-show="!showEdit"
              flat
              label="Add"
              color="info"
              @click="addPosition"
              :loading="positionProcess"
            />
            <q-btn
              v-show="showEdit"
              flat
              label="Edit"
              color="info"
              @click="editPosition"
              :loading="positionProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <div class="table-component full-height full-width flex flex-center q-px-md">
        <div class="q-mb-md full-width" style="height: 36px">
          <div class="full-width flex justify-end">
            <q-btn color="primary" label="New Position" @click="openAddPosition" />
          </div>
        </div>

        <q-table
          class="table-content"
          :rows="tempListPosition"
          :columns="headerTable"
          row-key="id"
          flat
          bordered
          dark
          :loading="loadingData"
          v-model:pagination="pagination"
          :rows-per-page-options="[5, 10, 15, 20]"
        >
          <template v-slot:header-cell-name="props">
            <q-th :props="props">
              <div :style="`min-width: ${widthName}`">
                <q-input
                  @update:model-value="filterNamePosition"
                  dark
                  dense
                  standout
                  v-model="filterModel.name"
                  input-class="text-right"
                  :label="labelNameFocus[0]"
                  :label-color="labelColorFocus[0]"
                  @focus="
                    labelColorFocus[0] = 'black';
                    labelNameFocus[0] = 'Search by name';
                    widthName = '200px';
                  "
                  @blur="
                    labelColorFocus[0] = 'white';
                    labelNameFocus[0] = props.col.label;
                    widthName = '130px';
                  "
                >
                  <template v-slot:append>
                    <q-icon v-if="!filterModel.name" name="search" />
                    <q-icon
                      v-else
                      name="clear"
                      class="cursor-pointer"
                      @click="
                        filterModel.name = '';
                        filterNamePosition();
                      "
                    />
                  </template>
                </q-input>
              </div>
            </q-th>
          </template>

          <template v-slot:body-cell-action="props">
            <q-td :props="props">
              <div>
                <q-btn
                  style="width: 60px"
                  dense
                  color="white"
                  text-color="black"
                  label="Edit"
                  @click="openEditPosition(props.value)"
                />
              </div>
              <div class="q-mt-sm">
                <q-btn
                  style="width: 60px"
                  dense
                  color="negative"
                  label="delete"
                  @click="
                    idDelete = props.value;
                    showDelete = true;
                  "
                />
              </div>
            </q-td>
          </template>

          <template v-slot:loading>
            <q-inner-loading showing color="primary" />
          </template>
        </q-table>
      </div>
    </div>
  </q-page>
</template>

<script>
import { defineComponent } from "vue";
import { mapActions } from "vuex";
import { useQuasar } from "quasar";
import { api } from "src/boot/axios";

export default defineComponent({
  name: "Position",

  data() {
    return {
      labelColorFocus: ["white", "white"],
      colorFocusCategory: ["", ""],
      labelNameFocus: ["Category", "Skills"],

      widthName: "130px",

      loadingData: false,

      showDelete: false,
      idDelete: null,
      deleteProcess: false,

      showPosition: false,
      positionProcess: false,

      idEdit: null,
      showEdit: false,

      tempPositionResource: {
        name: "",
      },

      filterModel: {
        name: "",
      },

      pagination: {
        page: 1,
        rowsPerPage: 10,
      },

      tempListPosition: [],
      listPosition: [],
      headerTable: [
        {
          name: "index",
          label: "#",
          align: "center",
          field: "index",
        },
        {
          name: "name",
          align: "left",
          label: "Name",
          field: (row) => row.name,
        },
        {
          name: "action",
          align: "center",
          label: "Actions",
          field: (row) => row.id,
        },
      ],
    };
  },
  methods: {
    ...mapActions("auth", ["validateToken"]),

    async getAllPosition() {
      try {
        this.loadingData = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .get(`/api/v1/position`)
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
          this.listPosition = [...result.resource];

          this.listPosition.forEach((row, index) => {
            row.index = index + 1;
          });

          this.tempListPosition = [...this.listPosition];
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
    filterNamePosition(val) {
      let namePositionSearch = val ? val?.trim().toUpperCase() : null;
      let addressPositionSearch = this.filterModel?.address?.trim().toUpperCase();

      if (namePositionSearch && addressPositionSearch) {
        this.tempListPosition = this.listPosition.filter(
          (x) =>
            x.name?.toUpperCase().includes(namePositionSearch) &&
            x.address?.toUpperCase().includes(addressPositionSearch)
        );
      } else if (namePositionSearch) {
        this.tempListPosition = this.listPosition.filter((x) =>
          x.name?.toUpperCase().includes(namePositionSearch)
        );
      } else if (addressPositionSearch) {
        this.tempListPosition = this.listPosition.filter((x) =>
          x.address?.toUpperCase().includes(addressPositionSearch)
        );
      } else if (!namePositionSearch && !addressPositionSearch) {
        this.tempListPosition = [...this.listPosition];
      }

      this.tempListPosition.forEach((row, index) => {
        row.index = index + 1;
      });
    },
    async deletePosition() {
      try {
        this.deleteProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .delete(`/api/v1/position/${this.idDelete}`)
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
          let index = this.listPosition.findIndex((x) => x.id == this.idDelete);
          let indexTemp = this.tempListPosition.findIndex(
            (x) => x.id == this.idDelete
          );

          // Original object
          this.listPosition.splice(index, 1);
          // Temp object
          this.tempListPosition.splice(indexTemp, 1);

          this.tempListPosition.forEach((row, index) => {
            row.index = index + 1;
          });

          this.showDelete = false;

          this.$q.notify({
            type: "positive",
            message: "Successfully deleted!",
          });
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.deleteProcess = false;
        this.showDelete = false;
      }
    },
    async addPosition() {
      try {
        if (
          !this.$refs.addCategoryOneRef.validate()
        )
          return null;

        this.positionProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .post(`/api/v1/position`, this.tempPositionResource)
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
          this.tempListPosition.unshift(result.resource);
          this.listPosition.push(result.resource);

          this.tempListPosition.forEach((row, index) => {
            row.index = index + 1;
          });

          this.tempPositionResource.name = "";

          this.$q.notify({
            type: "positive",
            message: "Successfully added!",
          });
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.positionProcess = false;
      }
    },
    openAddPosition() {
      this.showEdit = false;

      this.tempPositionResource.name = "";

      this.showPosition = true;
    },
    async editPosition() {
      try {
        if (
          !this.$refs.addCategoryOneRef.validate()
        )
          return null;

        this.positionProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .put(`/api/v1/position/${this.idEdit}`, this.tempPositionResource)
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
          let index = this.listPosition.findIndex((x) => x.id == this.idEdit);
          let indexTemp = this.tempListPosition.findIndex(
            (x) => x.id == this.idEdit
          );

          // Original object
          this.listPosition[index].name = result.resource.name;
          // Temp object
          this.tempListPosition[indexTemp].name = result.resource.name;

          this.showPosition = false;

          this.$q.notify({
            type: "positive",
            message: "Successfully edited!",
          });
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.positionProcess = false;
      }
    },
    openEditPosition(id) {
      this.idEdit = id;
      this.showEdit = true;
      let tempPosition = this.tempListPosition.find((x) => x.id == id);

      this.tempPositionResource.name = tempPosition.name;

      this.showPosition = true;
    },
  },
  computed: {
    getNameDelete() {
      let tempCategory = this.listPosition.filter((x) => x.id == this.idDelete);

      return tempCategory[0]?.name;
    },
    getPersistentPosition() {
      return this.tempPositionResource.name ? true : false;
    },
  },
  async created() {
    let isValid = await this.validateToken();
    if (!isValid) this.$router.replace("/login");

    await Promise.all([this.getAllPosition()]);
  },
  mounted() {
    const $q = useQuasar();
  },
});
</script>

<style lang="scss" scoped>
.container {
  position: relative;
  .table-component {
    position: relative;
    .table-content {
      /* height or max-height is important */
      height: 600px;
      width: 100%;
    }
  }
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
    z-index: 99;
    font-size: 14px !important;
    font-family: Poppins-Medium !important;
  }

  tbody tr td {
    font-size: 14px !important;
  }

  thead tr:first-child th {
    top: 0;
  }

  /* this is when the loading indicator appears */
  &.q-table--loading thead tr:last-child th {
    /* height of all previous header rows */
    top: 0;
  }
}

/* Scroll bar */
::-webkit-scrollbar {
  width: 10px;
  height: 10px;
}

::-webkit-scrollbar-thumb {
  background: $accent;
}

::-webkit-scrollbar-corner {
  background: $grey;
}
</style>
