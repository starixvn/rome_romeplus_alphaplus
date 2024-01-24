'use strict';

import gulp from 'gulp';
import { deleteSync } from 'del';
import * as dartSass from 'sass';
import gulpSass from 'gulp-sass';
import postcss from 'gulp-postcss';
import autoprefixer from 'autoprefixer';
import concat from 'gulp-concat';
import cssmin from 'gulp-cssmin';
import uglify from 'gulp-uglify';
import merge from 'merge-stream';
import ts from 'gulp-typescript';
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
    deleteSync('wwwroot/**/*');
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

function minCss() {
    const tasks = getBundles(regex.css).map(function (bundle) {
        return gulp
            .src(bundle.inputFiles, { base: '.' })
            .pipe(concat(bundle.outputFileName))
            .pipe(cssmin())
            .pipe(gulp.dest('.'));
    });
    return merge(tasks);
}

function copyImages() {
    return gulp.src('assets/styles/images/**/*').pipe(gulp.dest('wwwroot/bundles/images'));
}

function compileJs() {
    const tsProject = ts.createProject('tsconfig.json');
    return gulp
        .src('assets/scripts/**/*.ts')
        .pipe(tsProject())
        .pipe(
            gulp.dest(function (f) {
                return f.base;
            })
        );
}

function minJs() {
    const tasks = getBundles(regex.js).map(function (bundle) {
        return gulp
            .src(bundle.inputFiles, { base: '.' })
            .pipe(concat(bundle.outputFileName))
            .pipe(uglify())
            .pipe(gulp.dest('.'));
    });
    return merge(tasks);
}

gulp.task('clean', clean);

gulp.task(
    'default',
    gulp.series(clean, gulp.parallel(compileCss, compileJs), gulp.parallel([minCss, minJs, copyImages]))
);

gulp.task('watch', () => {
    gulp.watch(
        ['assets/styles/**/*.scss', 'assets/scripts/**/*.ts'],
        gulp.series(clean, gulp.parallel(compileCss, compileJs), gulp.parallel([minCss, minJs, copyImages]))
    );
});
