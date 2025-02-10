'use strict';

import gulp from 'gulp';
import concat from 'gulp-concat';
import * as dartSass from 'sass';
import gulpSass from 'gulp-sass';
import cssmin from 'gulp-cssmin';
import postcss from 'gulp-postcss';
import autoprefixer from 'autoprefixer';
import uglify from 'gulp-uglify';
import merge from 'merge-stream';
import bundleConfig from './bundleconfig.json' with { type: 'json' };
import { deleteSync } from 'del';
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
    deleteSync('wwwroot/assets/bundles/**/*');
    cb();
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

function minJs() {
    var tasks = getBundles(regex.js).map(function (bundle) {
        return gulp
            .src(bundle.inputFiles, { base: '.' })
            .pipe(concat(bundle.outputFileName))
            .pipe(uglify())
            .pipe(gulp.dest('.'));
    });
    return merge(tasks);
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

function copyImages(){
    return gulp.src('assets/styles/images/**/*')
        .pipe(gulp.dest('wwwroot/assets/bundles/images'));
}

function copyFonts(){
    return gulp.src('assets/styles/fonts/**/*')
        .pipe(gulp.dest('wwwroot/assets/bundles/fonts'));
}

gulp.task('default', gulp.series(clean, compileCss, gulp.parallel(minCss, minJs, copyImages, copyFonts)));
