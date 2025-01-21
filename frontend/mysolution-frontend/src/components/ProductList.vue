<template>
  <div>
    <h2>Product List</h2>
    <input v-model="filter" placeholder="Filter by title" @input="applyFilter" />
    <table>
      <thead>
        <tr>
          <th>ID</th>
          <th>Title</th>
          <th>Price</th>
          <th>Category</th>
          <th>Rating</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="product in filteredProducts" :key="product.id">
          <td>{{ product.id }}</td>
          <td>{{ product.title }}</td>
          <td>{{ product.price }}</td>
          <td>{{ product.category }}</td>
          <td>{{ product.rating }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted, computed } from 'vue';
import { getAllProducts } from '@/services/ProductService';

interface Product {
  id: number;
  title: string;
  price: number;
  description: string;
  category: string;
  imageUrl: string;
  rating: number;
  ratingCount: number;
}

export default defineComponent({
  name: 'ProductList',
  setup() {
    const products = ref<Product[]>([]);
    const filter = ref('');

    const fetchProducts = async () => {
      products.value = await getAllProducts();
    };

    onMounted(() => {
      fetchProducts();
    });

    const filteredProducts = computed(() => {
      if (!filter.value) return products.value;
      return products.value.filter(p =>
        p.title.toLowerCase().includes(filter.value.toLowerCase())
      );
    });

    const applyFilter = () => {
      // a computed já filtra, então aqui não precisa fazer nada extra
    };

    return {
      products,
      filter,
      filteredProducts,
      applyFilter
    };
  }
});
</script>

<style scoped>
table {
  border-collapse: collapse;
  width: 100%;
}

th, td {
  border: 1px solid #ccc;
  padding: 8px;
}

input {
  margin-bottom: 8px;
  padding: 4px;
  width: 300px;
}
</style>
