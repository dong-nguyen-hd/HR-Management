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
              @click="deleteCategory"
              :loading="deleteProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <q-dialog v-model="showCategory" :persistent="getPersistentCategory">
        <q-card style="width: 400px">
          <q-card-section class="row items-center">
            <span v-show="!showEdit" class="text-h6">Add Category</span>
            <span v-show="showEdit" class="text-h6">Edit Category</span>
          </q-card-section>

          <q-separator />

          <q-card-section>
            <div class="q-mt-sm">
              <q-input
                ref="addCategoryOneRef"
                standout
                clearable
                maxlength="250"
                v-model="tempCategoryResource.name"
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

            <div class="q-mt-sm">
              <q-select
                ref="addCategoryTwoRef"
                standout
                hide-dropdown-icon
                multiple
                v-model="tempCategoryResource.technologies"
                label="Skills"
                option-label="name"
                @new-value="createSkillBelongWithCategory"
                input-debounce="0"
                use-input
                use-chips
                :label-color="colorFocusCategory[1]"
                @focus="colorFocusCategory[1] = 'white'"
                @blur="colorFocusCategory[1] = ''"
              >
              </q-select>
            </div>
          </q-card-section>

          <q-card-actions align="right">
            <q-btn
              flat
              :disable="categoryProcess"
              label="Cancel"
              color="primary"
              v-close-popup
            />
            <q-btn
              v-show="!showEdit"
              flat
              label="Add"
              color="info"
              @click="addCategory"
              :loading="categoryProcess"
            />
            <q-btn
              v-show="showEdit"
              flat
              label="Edit"
              color="info"
              @click="editCategory"
              :loading="categoryProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <div class="table-component full-height full-width flex flex-center q-px-md">
        <div class="q-mb-md full-width" style="height: 36px">
          <div class="full-width flex justify-end">
            <q-btn
              color="primary"
              label="New Category"
              @click="openAddCategory"
            />
          </div>
        </div>

        <q-table
          class="table-content"
          :rows="listCategory"
          :columns="headerTable"
          row-key="id"
          flat
          bordered
          dark
          :loading="loadingData"
          v-model:pagination="pagination"
          :rows-per-page-options="[5, 10, 15, 20]"
          @request="getCategoryWithFilter"
        >
          <template v-slot:header-cell-category="props">
            <q-th :props="props">
              <div :style="`min-width: ${widthCategory}`">
                <q-input
                  @update:model-value="getCategoryWithFilter(false)"
                  dark
                  dense
                  standout
                  v-model="filter.categoryName"
                  input-class="text-right"
                  :label="labelNameFocus[0]"
                  :label-color="labelColorFocus[0]"
                  debounce="300"
                  @focus="
                    labelColorFocus[0] = 'black';
                    labelNameFocus[0] = 'Search by category name';
                    widthCategory = '200px';
                  "
                  @blur="
                    labelColorFocus[0] = 'white';
                    labelNameFocus[0] = props.col.label;
                    widthCategory = '130px';
                  "
                >
                  <template v-slot:append>
                    <q-icon v-if="!filter.categoryName" name="search" />
                    <q-icon
                      v-else
                      name="clear"
                      class="cursor-pointer"
                      @click="
                        filter.categoryName = '';
                        getCategoryWithFilter(false);
                      "
                    />
                  </template>
                </q-input>
              </div>
            </q-th>
          </template>

          <template v-slot:header-cell-skill="props">
            <q-th :props="props">
              <div :style="`min-width: ${widthSkill}`">
                <q-input
                  @update:model-value="getCategoryWithFilter(false)"
                  dark
                  dense
                  standout
                  v-model="filter.technologyName"
                  input-class="text-right"
                  :label="labelNameFocus[1]"
                  :label-color="labelColorFocus[1]"
                  debounce="300"
                  @focus="
                    labelColorFocus[1] = 'black';
                    labelNameFocus[1] = 'Search by skill name';
                    widthCategory = '154px';
                  "
                  @blur="
                    labelColorFocus[1] = 'white';
                    labelNameFocus[1] = props.col.label;
                    widthCategory = '110px';
                  "
                >
                  <template v-slot:append>
                    <q-icon v-if="!filter.technologyName" name="search" />
                    <q-icon
                      v-else
                      name="clear"
                      class="cursor-pointer"
                      @click="
                        filter.technologyName = '';
                        getCategoryWithFilter(false);
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
                  @click="openEditCategory(props.value)"
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

          <template v-slot:body-cell-category="props">
            <q-td :props="props">
              {{ showName(props.value) }}
            </q-td>
          </template>

          <template v-slot:body-cell-skill="props">
            <q-td :props="props">
              <div v-if="props.value?.length" class="row">
                <q-badge
                  v-for="(item, index) in props.value"
                  :key="index"
                  :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                  style="font-size: 14px"
                  class="q-mr-sm q-mt-sm"
                >
                  {{ props?.value[index].name }}
                </q-badge>
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
  name: "Category",

  data() {
    return {
      labelColorFocus: ["white", "white"],
      colorFocusCategory: ["", ""],
      labelNameFocus: ["Category", "Skills"],

      widthCategory: "130px",
      widthSkill: "110px",

      loadingData: false,

      showDelete: false,
      idDelete: null,
      deleteProcess: false,

      showCategory: false,
      categoryProcess: false,

      idEdit: null,
      showEdit: false,

      tempCategoryResource: {
        name: "",
        technologies: [],
      },

      filter: {
        categoryName: null,
        technologyName: null,
      },

      pagination: {
        page: 1,
        rowsPerPage: 10,
        rowsNumber: 10, // is total records

        firstPage: 1,
        lastPage: null,
        nextPage: null,
        previousPage: null,
        totalPages: 0,
      },

      listCategory: [],
      headerTable: [
        {
          name: "index",
          label: "#",
          align: "center",
          field: "index",
        },
        {
          name: "category",
          align: "left",
          label: "Category",
          field: (row) => row.name,
        },
        {
          name: "skill",
          align: "left",
          label: "Skills",
          field: (row) => row.technologies,
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

    async getCategoryWithFilter(props) {
      try {
        this.loadingData = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        let { page, rowsPerPage } = props
          ? props.pagination
          : { page: 1, rowsPerPage: this.pagination.rowsPerPage };

        // Request API
        let result = await api
          .post(
            `/api/v1/category/pagination?page=${page}&pageSize=${rowsPerPage}`,
            this.filter
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
          this.mappingPagination(result);
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
    createSkillBelongWithCategory(val, done) {
      if (val.length > 2) {
        let temp = { name: val.trim(), categoryId: 0 };
        done(temp, "add-unique");
      }
    },
    mappingPagination(resource) {
      this.listCategory = resource.resource;

      this.listCategory.forEach((row, index) => {
        row.index = index + 1;
      });

      this.pagination.page = resource.page;
      this.pagination.rowsPerPage = resource.pageSize;
      this.pagination.rowsNumber = resource.totalRecords;

      this.pagination.firstPage = resource.firstPage;
      this.pagination.lastPage = resource.lastPage;
      this.pagination.nextPage = resource.nextPage;
      this.pagination.previousPage = resource.previousPage;
      this.pagination.totalPages = resource.totalPages;
    },
    showName(text) {
      if (text.length > 20) return `${texttext.slice(0, 20)}...`;
      else return text;
    },
    async deleteCategory() {
      try {
        this.deleteProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .delete(`/api/v1/category/${this.idDelete}`)
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
          await this.getCategoryWithFilter(false);

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
    async addCategory() {
      try {
        if (!this.$refs.addCategoryOneRef.validate()) return null;

        this.categoryProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .post(`/api/v1/category`, this.tempCategoryResource)
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
          this.listCategory.unshift(result.resource);

          this.listCategory.forEach((row, index) => {
            row.index = index + 1;
          });

          this.tempCategoryResource.name = "";
          this.tempCategoryResource.technologies = [];

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
        this.categoryProcess = false;
      }
    },
    openAddCategory() {
      this.showEdit = false;

      this.tempCategoryResource.name = "";
      this.tempCategoryResource.technologies = [];

      this.showCategory = true;
    },
    async editCategory() {
      try {
        if (!this.$refs.addCategoryOneRef.validate()) return null;

        this.categoryProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .put(`/api/v1/category/${this.idEdit}`, this.tempCategoryResource)
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
          let index = this.listCategory.findIndex(x => x.id == this.idEdit)
          this.listCategory[index].name = result.resource.name;
          this.listCategory[index].technologies = result.resource.technologies;

          this.showCategory = false;

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
        this.categoryProcess = false;
      }
    },
    openEditCategory(id) {
      this.idEdit = id;
      this.showEdit = true;
      let tempCategory = this.listCategory.find(x => x.id == id)

      this.tempCategoryResource.name = tempCategory.name;
      this.tempCategoryResource.technologies = tempCategory.technologies;

      this.showCategory = true;
    },
  },
  computed: {
    getNameDelete() {
      let tempCategory = this.listCategory.filter((x) => x.id == this.idDelete);

      let name = tempCategory[0]?.name;

      return name ? this.showName(name) : "";
    },
    getPersistentCategory() {
      return this.tempCategoryResource.name ||
        this.tempCategoryResource.technologies?.length
        ? true
        : false;
    },
  },
  async created() {
    let isValid = await this.validateToken();
    if (!isValid) this.$router.replace("/login");

    await Promise.all([this.getCategoryWithFilter()]);
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
