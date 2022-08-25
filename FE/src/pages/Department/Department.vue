<template>
  <q-page class="full-height full-width flex flex-center">
    <div class="container full-height full-width">
      <q-dialog v-model="showDelete" :persistent="deleteProcess">
        <q-card>
          <q-card-section class="row items-center">
            <span class="text-h6">{{$t('delete')}} {{ getNameDelete }}?</span>
          </q-card-section>

          <q-separator />

          <q-card-section>
            <span
              >{{$t('confirmDelete')}}</span
            >
          </q-card-section>

          <q-card-actions align="right">
            <q-btn
              flat
              :disable="deleteProcess"
              :label="$t('cancel')"
              color="primary"
              v-close-popup
            />
            <q-btn
              flat
              :label="$t('btnDelete')"
              color="negative"
              @click="deleteDepartment"
              :loading="deleteProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <q-dialog v-model="showDepartment" :persistent="getPersistentDepartment">
        <q-card style="width: 400px">
          <q-card-section class="row items-center">
            <span v-show="!showEdit" class="text-h6">{{$t('addDepartment')}}</span>
            <span v-show="showEdit" class="text-h6">{{$t('editDepartment')}}</span>
          </q-card-section>

          <q-separator />

          <q-card-section>
            <div class="q-mt-sm">
              <q-input
                ref="addCategoryOneRef"
                standout
                clearable
                maxlength="250"
                v-model="tempDepartmentResource.name"
                type="text"
                :label="$t('nameColon')"
                :label-color="colorFocusCategory[0]"
                @focus="colorFocusCategory[0] = 'white'"
                @blur="colorFocusCategory[0] = ''"
                :rules="[(val) => !!val || $t('nameRequired')]"
                lazy-rules="ondemand"
                hide-bottom-space
              >
              </q-input>
            </div>
          </q-card-section>

          <q-card-actions align="right">
            <q-btn
              flat
              :disable="departmentProcess"
              :label="$t('cancel')"
              color="primary"
              v-close-popup
            />
            <q-btn
              v-show="!showEdit"
              flat
              :label="$t('btnAdd')"
              color="info"
              @click="addDepartment"
              :loading="departmentProcess"
            />
            <q-btn
              v-show="showEdit"
              flat
              :label="$t('btnEdit')"
              color="info"
              @click="editDepartment"
              :loading="departmentProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <div class="table-component full-height full-width flex flex-center q-px-md">
        <div class="q-mb-md full-width" style="height: 36px">
          <div class="full-width flex justify-end">
            <q-btn color="primary" :label="$t('newDepartment')" @click="openAddDepartment" />
          </div>
        </div>

        <q-table
          class="table-content"
          :rows="tempListDepartment"
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
                  @update:model-value="filterNameDepartment"
                  dark
                  dense
                  standout
                  v-model="filterModel.name"
                  input-class="text-right"
                  :label="labelNameFocus[0]"
                  :label-color="labelColorFocus[0]"
                  @focus="
                    labelColorFocus[0] = 'black';
                    labelNameFocus[0] = $t('searchByName');
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
                        filterNameDepartment();
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
                  :label="$t('btnEdit')"
                  @click="openEditDepartment(props.value)"
                />
              </div>
              <div class="q-mt-sm">
                <q-btn
                  style="width: 60px"
                  dense
                  color="negative"
                  :label="$t('btnDelete')"
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
  name: "Department",

  data() {
    return {
      labelColorFocus: ["white", "white"],
      colorFocusCategory: ["", ""],
      labelNameFocus: [this.$t('category'), this.$t('skillLower')],

      widthName: "130px",

      loadingData: false,

      showDelete: false,
      idDelete: null,
      deleteProcess: false,

      showDepartment: false,
      departmentProcess: false,

      idEdit: null,
      showEdit: false,

      tempDepartmentResource: {
        name: "",
      },

      filterModel: {
        name: "",
      },

      pagination: {
        page: 1,
        rowsPerPage: 10,
      },

      tempListDepartment: [],
      listDepartment: [],
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
          label: this.$t('name'),
          field: (row) => row.name,
        },
        {
          name: "action",
          align: "center",
          label: this.$t('actions'),
          field: (row) => row.id,
        },
      ],
    };
  },
  methods: {
    ...mapActions("auth", ["validateToken"]),

    async getAllDepartment() {
      try {
        this.loadingData = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .get(`/api/v1/department`)
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
          this.listDepartment = [...result.resource];

          this.listDepartment.forEach((row, index) => {
            row.index = index + 1;
          });

          this.tempListDepartment = [...this.listDepartment];
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
    filterNameDepartment(val) {
      let nameDepartmentSearch = val ? val?.trim().toUpperCase() : null;
      let addressDepartmentSearch = this.filterModel?.address?.trim().toUpperCase();

      if (nameDepartmentSearch && addressDepartmentSearch) {
        this.tempListDepartment = this.listDepartment.filter(
          (x) =>
            x.name?.toUpperCase().includes(nameDepartmentSearch) &&
            x.address?.toUpperCase().includes(addressDepartmentSearch)
        );
      } else if (nameDepartmentSearch) {
        this.tempListDepartment = this.listDepartment.filter((x) =>
          x.name?.toUpperCase().includes(nameDepartmentSearch)
        );
      } else if (addressDepartmentSearch) {
        this.tempListDepartment = this.listDepartment.filter((x) =>
          x.address?.toUpperCase().includes(addressDepartmentSearch)
        );
      } else if (!nameDepartmentSearch && !addressDepartmentSearch) {
        this.tempListDepartment = [...this.listDepartment];
      }

      this.tempListDepartment.forEach((row, index) => {
        row.index = index + 1;
      });
    },
    async deleteDepartment() {
      try {
        this.deleteProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .delete(`/api/v1/department/${this.idDelete}`)
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
          let index = this.listDepartment.findIndex((x) => x.id == this.idDelete);
          let indexTemp = this.tempListDepartment.findIndex(
            (x) => x.id == this.idDelete
          );

          // Original object
          this.listDepartment.splice(index, 1);
          // Temp object
          this.tempListDepartment.splice(indexTemp, 1);

          this.tempListDepartment.forEach((row, index) => {
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
    async addDepartment() {
      try {
        if (
          !this.$refs.addCategoryOneRef.validate()
        )
          return null;

        this.departmentProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .post(`/api/v1/department`, this.tempDepartmentResource)
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
          this.tempListDepartment.unshift(result.resource);
          this.listDepartment.push(result.resource);

          this.tempListDepartment.forEach((row, index) => {
            row.index = index + 1;
          });

          this.tempDepartmentResource.name = "";

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
        this.departmentProcess = false;
      }
    },
    openAddDepartment() {
      this.showEdit = false;

      this.tempDepartmentResource.name = "";

      this.showDepartment = true;
    },
    async editDepartment() {
      try {
        if (
          !this.$refs.addCategoryOneRef.validate()
        )
          return null;

        this.departmentProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .put(`/api/v1/department/${this.idEdit}`, this.tempDepartmentResource)
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
          let index = this.listDepartment.findIndex((x) => x.id == this.idEdit);
          let indexTemp = this.tempListDepartment.findIndex(
            (x) => x.id == this.idEdit
          );

          // Original object
          this.listDepartment[index].name = result.resource.name;
          // Temp object
          this.tempListDepartment[indexTemp].name = result.resource.name;

          this.showDepartment = false;

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
        this.departmentProcess = false;
      }
    },
    openEditDepartment(id) {
      this.idEdit = id;
      this.showEdit = true;
      let tempDepartment = this.tempListDepartment.find((x) => x.id == id);

      this.tempDepartmentResource.name = tempDepartment.name;

      this.showDepartment = true;
    },
  },
  computed: {
    getNameDelete() {
      let tempCategory = this.listDepartment.filter((x) => x.id == this.idDelete);

      return tempCategory[0]?.name;
    },
    getPersistentDepartment() {
      return this.tempDepartmentResource.name ? true : false;
    },
  },
  async created() {
    let isValid = await this.validateToken();
    if (!isValid) this.$router.replace("/login");

    await Promise.all([this.getAllDepartment()]);
  },
  mounted() {
    const $q = useQuasar();
  },
});
</script>

<style lang="scss" scoped>
.container {
  department: relative;
  .table-component {
    department: relative;
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
    department: sticky;
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
