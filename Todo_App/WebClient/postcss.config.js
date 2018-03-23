module.exports = (ctx) => ({
    plugins: {
        'postcss-import': {},
        'postcss-cssnext': { browsers: ['last 2 Chrome version'] }
    }
})