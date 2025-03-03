# Use the official node image to build the application
FROM node:20-alpine AS build
WORKDIR /app

# Copy the package files
COPY package*.json .

# Install dependencies
RUN npm install

# Copy everything else
COPY . .

# Build dist
RUN npm run build

# Start production image build
FROM node:20-alpine AS production
WORKDIR /app

# Copy the package files
COPY package*.json .

# Install production dependencies
RUN npm ci --only=production

# Copy build directory
COPY --from=build /app/dist ./dist

# Run server
EXPOSE 3001
CMD ["node", "dist/index.js"]