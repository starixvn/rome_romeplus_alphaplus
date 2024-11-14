'use strict';

import gulp from 'gulp';
import { deleteSync } from 'del';
import concat from 'gulp-concat';
import * as dartSass from 'sass';
import gulpSass from 'gulp-sass';
import cssmin from 'gulp-cssmin';
import postcss from 'gulp-postcss';
import autoprefixer from 'autoprefixer';
import uglify from 'gulp-uglify';
import merge from 'merge-stream';
import bundleConfig from './bundleconfig.json' assert { type: 'json' };
const sass = gulpSass(dartSass);

const regex = {
    css: /\.css$/,
    js: /\.js$/
};

function getBundles(regexPattern) {
    return bundleConfig.filter(function (bundle) {
        return regexPattern.test(bundle.outputFileName);
    });
}

function clean(cb) {
    //deleteSync('wwwroot/assets/bundles/**/*');
    cb();
}

function minJs() {
    var tasks = getBundles(regex.js).map(function (bundle) {
        return gulp
            .src(bundle.inputFiles, { base: '.' })
            .pipe(concat(bundle.outputFileName))
            // .pipe(uglify())
            .pipe(gulp.dest('.'));
    });
    return merge(tasks);
}

function compileCss() {
    return gulp
        .src('assets/styles/**/*.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(postcss([autoprefixer()]))
        .pipe(
            gulp.dest(function (f) {
                return f.base;
            })
        );
}

function minCss() {
    var tasks = getBundles(regex.css).map(function (bundle) {
        return gulp
            .src(bundle.inputFiles, { base: '.' })
            .pipe(concat(bundle.outputFileName))
            .pipe(cssmin())
            .pipe(gulp.dest('.'));
    });
    return merge(tasks);
}

gulp.task('default', gulp.series(clean, compileCss, gulp.parallel(minCss, minJs)));
