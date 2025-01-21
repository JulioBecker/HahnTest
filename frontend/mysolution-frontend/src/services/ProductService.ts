import axios from 'axios';

const API_URL = 'http://localhost:5000/api';

export async function getAllProducts() {
  const response = await axios.get(`${API_URL}/products`);
  return response.data;
}
