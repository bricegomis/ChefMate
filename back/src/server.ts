import express, { Request, Response } from 'express';
import mongoose from 'mongoose';
import dotenv from 'dotenv';

dotenv.config();
const app = express();
app.use(express.json());

// Connect to MongoDB Atlas
mongoose.connect(process.env.MONGO_URI as string, {
  useNewUrlParser: true,
  useUnifiedTopology: true
}).then(() => console.log('MongoDB connected')).catch(err => console.log(err));

// Ingredient model
interface PurchasePrice {
  price: number;
  type: string;  // Dynamically defined, allowing any string
  purchaseDate: Date;
  store: string;
}

interface Ingredient {
  name: string;
  purchasePrices: PurchasePrice[];
}

const ingredientSchema = new mongoose.Schema<Ingredient>({
  name: { type: String, required: true },
  purchasePrices: [{
    price: { type: Number, required: true },
    type: { type: String, required: true },  // No predefined values, any string can be added
    purchaseDate: { type: Date, required: true },
    store: { type: String, required: true }
  }]
});
const IngredientModel = mongoose.model<Ingredient>('Ingredient', ingredientSchema);

// CRUD Routes
app.get('/ingredients', async (req: Request, res: Response) => {
  const ingredients = await IngredientModel.find();
  res.json(ingredients);
});

app.post('/ingredients', async (req: Request, res: Response) => {
  const ingredient = new IngredientModel(req.body);
  await ingredient.save();
  res.json(ingredient);
});

app.put('/ingredients/:id', async (req: Request, res: Response) => {
  const ingredient = await IngredientModel.findByIdAndUpdate(req.params.id, req.body, { new: true });
  res.json(ingredient);
});

app.delete('/ingredients/:id', async (req: Request, res: Response) => {
  await IngredientModel.findByIdAndDelete(req.params.id);
  res.json({ message: 'Ingredient deleted' });
});

const PORT = process.env.PORT || 3001;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
