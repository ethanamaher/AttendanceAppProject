// tailwind.config.js
module.exports = {
    content: [
        './**/*.razor', // Adjust paths to your project structure
    ],
    safelist: [
        'font-neuton' // ← Forces this class to be generated
    ],
    theme: {
        fontFamily: {
            'neuton': ['Neuton', 'serif'],
        },
    },
}