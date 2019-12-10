const path = require('path');
const CopyPlugin = require('copy-webpack-plugin');

module.exports = {
    entry: './src/index.tsx',
    mode: 'development',
    output: {
        filename: 'bundle.js',
        path: path.resolve(__dirname, 'dist'),
    },
    devtool: 'inline-source-map',
    devServer: {
        contentBase: './dist',
        hot: true,
        inline: true,
        compress: true,
        historyApiFallback: true
    },
    plugins: [
        new CopyPlugin([
            { from: './_framework', to: './_framework' },
            { from: './index.html', to: './' },
        ]),
    ],
    module: {
        rules: [
            {
                test: /\.css$/,
                use: [
                    'style-loader',
                    'css-loader',
                ],
                exclude: /node_modules/
            }, {
                test: /\.(png|svg|jpg|gif)$/,
                use: [
                    'file-loader',
                ],
                exclude: /node_modules/
            }, {
                test: /\.jsx?/i,
                loader: 'babel-loader',
                options: {
                    presets: ["@babel/preset-env"],
                    plugins: [
                        ['transform-react-jsx', {
                            pragma: 'h',
                            pragmaFrag: 'Fragment'
                        }]
                    ]
                }
            }, {
                test: /\.tsx?$/,
                use: 'ts-loader',
                exclude: /node_modules/
            },
        ]
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.js'],
    },
};