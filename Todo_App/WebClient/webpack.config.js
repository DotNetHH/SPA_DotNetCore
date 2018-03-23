const path = require('path');
const webpack = require('webpack');
const CleanWebpackPlugin = require('clean-webpack-plugin');

const package = require('./package.json');
const bundleOutputDir = '../WebAPI/wwwroot/dist';

module.exports = (env, argv) => {

    return {
        // WebpackMiddleware kann kein --mode oder argv setzten
        mode: argv && argv.mode ? argv.mode : 'development',
        stats: { modules: false },
        entry: { app: './src/init.ts' },
        output: {
            path: path.resolve(bundleOutputDir),
            publicPath: '/dist/',
            filename: 'build.js'
        },
        resolve: {
            extensions: ['.ts', '.js']
        },
        module: {
            rules: [
                {
                    test: /\.ts$/,
                    exclude: [/node_modules/, /cypress/],
                    loader: 'ts-loader',
                    options: { appendTsSuffixTo: [/\.vue$/] }
                },
                { test: /\.vue$/, loader: 'vue-loader', options: {} },
                { test: /\.css$/, use: ['css-loader', 'postcss-loader'] }
            ]
        },
        plugins: [
            new CleanWebpackPlugin([bundleOutputDir], { allowExternal: true }),
            new webpack.DefinePlugin({
                APP_VERSION: JSON.stringify(package.version)
            }),
        ]
    }
}
